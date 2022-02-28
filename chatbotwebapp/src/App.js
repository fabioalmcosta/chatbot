import logo from './logo.svg';
import './App.css';
import Chat from './Modules/Chat';
import { ChakraProvider } from '@chakra-ui/react';
import theme from './ColorMode';

function App() {
  return (
    <>
      <ChakraProvider theme={theme}>
        <Chat />
      </ChakraProvider>
    </>
  );
}

export default App;
