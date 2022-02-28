import React, { useState, useEffect, useRef } from 'react';
import {
  Icon,
  Input,
  Button,
  Center,
  Box,
  Flex, 
  Text,
} from '@chakra-ui/react';
import { FaUserAlt } from 'react-icons/fa';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { parseISO, format } from 'date-fns';
import { API_ROUTE } from './ApiRoute';

const Component = (props) => {
  const [chatUsers, setChatUsers] = useState([]);
  const [activeMessages, setActiveMessages] = useState([]);
  const [activeChat, setActiveChat] = useState();
  const [errorMessage, setErrorMessage] = useState("");
  const [message, setMessage] = useState("");
  const latestChat = useRef(null);
  const latestActiveChat = useRef(null);
  latestChat.current = activeMessages;
  latestActiveChat.current = activeChat;

  useEffect(() => {
    const connection = new HubConnectionBuilder()
        .withUrl(API_ROUTE + 'hubs/chat?user=' + JSON.stringify(props.token.Email))
        .withAutomaticReconnect()
        .build();

    connection.start()
        .then(result => {
            console.log('Connected!', result);

            connection.on("chat", (users, user) => {
                let usersConnected = [];
                users.map(u => {
                    if (u.id != props.token.Id) usersConnected.push(u);
                });
                setChatUsers(usersConnected);
                setActiveChat();
            });

            connection.on('Receive', (id, message) => {
                let updatedActiveChat = latestActiveChat.current;
                setActiveChat(updatedActiveChat);
                if (latestActiveChat.current == message.toId || latestActiveChat.current == message.fromId) {
                    let updatedChat = [...latestChat.current];
                    updatedChat.push(message);
                
                    setActiveMessages([...updatedChat]);                    
                }
            });
        })
        .catch(e => console.log('Connection failed: ', e));
}, []);

  const onSubmit = () => {
  const requestOptions = {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', 'Authorization': props.token.Token },
    body: JSON.stringify({ 
        ToId: activeChat, 
        Message: message})
    };
    fetch(API_ROUTE + 'chatmessage', requestOptions)
    .then(response => response.json())
    .then((data) => {
        if (data.message) {
            setErrorMessage(data.message);
        }else{
            let updatedChat = [...activeMessages];
            updatedChat.push(data);
        
            setActiveMessages([...updatedChat]);
            setMessage("");
        }
    });
  };

  const onClickUser = (id) => {
    const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json', 'Authorization': props.token.Token }
    };
    fetch(API_ROUTE + 'chatmessage/' + id, requestOptions)
        .then(response => response.json())
        .then((data) => {
            if (data.message) {
                setErrorMessage(data.message);
            } else {
                setActiveMessages([...data]);
                setActiveChat(id);
            }
        });
  };

  return (
   <>
   <Flex color='white'>
        <Center w='30%'  borderWidth="3px">
            <Text fontWeight='bold'>Live Users</Text>
        </Center>
        <Box flex='1' borderWidth="3px">
            <Text fontWeight='bold'>Chat Box</Text>
        </Box>
    </Flex>
    <Flex color='white'>
        <Box w='30%'  borderWidth="3px" textAlign="center">
            <ul style={{listStyleType: 'none', borderWidth: '1px'}}>
            {chatUsers.map((user) =>
                <a href='#' onClick={() => onClickUser(user.id)}>
                    <li key={user.id} style={{padding: '20px', backgroundColor: activeChat == user.id? 'gray' : '#1a202c'}}>
                        <Text><Icon as={FaUserAlt} /> {user.name} {user.surname}</Text>
                    </li>
                </a>
            )}
            </ul>
        </Box>
        <Box flex='1' borderWidth="3px">
        <ul style={{listStyleType: 'none', borderWidth: '1px'}}>
            {activeMessages.map((message) =>
                <li key={message.id} style={{margin: '20px'}}>
                    <Box  borderWidth='1px' borderRadius='lg' width='45%' 
                    marginLeft={activeChat == message.ToId || activeChat == message.toId ? 'auto' : 0}
                    marginRight={activeChat == message.ToId || activeChat == message.toId ? 0 : 'auto'}
                    textAlign={activeChat == message.ToId || activeChat == message.toId ? 'right' : 'left'}
                    backgroundColor={activeChat == message.ToId || activeChat == message.toId ? 'seagreen' : 'royalblue'}>
                        {(message.ToId == 0 || message.toId == 0) && (
                            <Text color='darkred'>This is a message from BOT</Text>                            
                        )}
                        <Text fontSize='xs'>{message.date ? format(parseISO(message.date), 'dd/mm/yyyy HH:mm:ss') : format(parseISO(message.Date), 'dd/mm/yyyy HH:mm:ss')}</Text>
                        <Text>{message.message ? message.message : message.Message}</Text>
                    </Box>
                </li>
            )}
            </ul>
        {activeChat && (
            <>
            <Flex color='white'>
                <Center w='75%'>
                    <Input
                        variant="outline"
                        name="Message"
                        type="text"
                        placeholder="Message"
                        value={message}
                        onChange={e => setMessage(e.target.value)}
                        isRequired
                    />
                </Center>
                <Center flex='1'>
                    <Button
                    colorScheme="blue"
                    variant="solid"
                    w="100%"
                    type="button"
                    onClick={() => onSubmit()}
                    >
                    Send
                    </Button>
                </Center>
            </Flex>
         </>
        )}
        </Box>
    </Flex>
   </>
  );
};

export default Component;
