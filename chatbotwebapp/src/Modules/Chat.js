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
import Login from './Login';
import CreateAccount from './RegisterNow';
import ChatModule from './ChatModule';

const Component = () => {
  const [token, setToken] = useState("");
  const [page, setPage] = useState(1);
  
  return (
   <>
   {page == 1 && (
       <Login setPage={setPage} setToken={setToken} />
   )}
   {page == 2 && (
       <CreateAccount setPage={setPage} setToken={setToken} />
   )}
   {page == 3 && (
       <ChatModule setPage={setPage} token={token}/>

   )}
   </>
  );
};

export default Component;
