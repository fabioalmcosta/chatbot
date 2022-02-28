import React, { useState } from 'react';
import {
  Icon,
  Input,
  Button,
  Checkbox,
  InputGroup,
  InputLeftElement,
  Center,
  Box,
} from '@chakra-ui/react';
import { FaUserAlt, FaLock } from 'react-icons/fa';
import { API_ROUTE } from './ApiRoute';

const Component = (props) => {
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const onSubmit = (values) => {
    values.preventDefault();
    setLoading(!loading);

    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Login: values.target.Login.value, Password: values.target.Password.value })
    };
    fetch(API_ROUTE + 'login', requestOptions)
        .then(response => response.json())
        .then((data) => {
            if (data.message) {
                setErrorMessage(data.message);
                document.getElementById("loginForm").reset();    
                setLoading(false);
            } else {
                props.setToken(data);
                props.setPage(3);
            }
        });
  };

  return (
    <Center margin="auto" w="100%" h="100%" marginTop="10%">
      <Box
        w={['100%', '75%', '50%', '30%']}
        p={4}
        borderWidth="1px"
        borderRadius="lg"
      >
        <form id="loginForm" name="loginForm" onSubmit={onSubmit}>
          <img
            src="https://picsum.photos/350/70"
            alt="Teste"
            width="100%"
            height="70px"
            style={{ marginBottom: '30px' }}
          />
           <Box mt={2} color={"red"}>
            {errorMessage}
          </Box>
          <InputGroup mt={2}>
            <InputLeftElement
              pointerEvents="none"
              children={<Icon as={FaUserAlt} />}
            />
            <Input
              variant="outline"
              name="Login"
              type="text"
              placeholder="Login"
              isRequired
            />
          </InputGroup>
          <InputGroup mt={2}>
            <InputLeftElement
              pointerEvents="none"
              children={<Icon as={FaLock} />}
            />
            <Input
              variant="outline"
              name="Password"
              type="password"
              placeholder="Password"
              isRequired
            />
          </InputGroup>
          <Button
            colorScheme="blue"
            variant="solid"
            isLoading={loading}
            mt={2}
            w="100%"
            type="submit"
          >
            Login
          </Button>
          <Box mt={2}>
            Or
            <a onClick={() => props.setPage(2)} href="#"> register now!</a>
          </Box>
        </form>
      </Box>
    </Center>
  );
};

export default Component;
