import {
  AsyncStorageGetObject,
  AsyncStorageSetObject,
} from '../helpers/AsyncStorageExtensions';

import { clienteMapper } from '../helpers/Mappers';
import HttpClientFactory from '../helpers/HttpClientFactory';

export const updateAddressOnStorage = async endereco => {
  const usuario = await AsyncStorageGetObject('client');

  usuario.enderecos.pop();
  usuario.enderecos.push(endereco);

  await AsyncStorageSetObject('client', usuario);
};

export const appAuthenticate = async cliente => {
  const HttpClient = await HttpClientFactory();

  const { data } = await HttpClient.post('Authentication', cliente);

  await AsyncStorageSetObject('client', data);
};

export const passwordRecovery = async email => {
  const HttpClient = await HttpClientFactory();

  return HttpClient.put(`Clientes/PasswordRecovery?email=${email}`);
};

export const postCliente = async cliente => {
  const HttpClient = await HttpClientFactory();

  const { data } = await HttpClient.post('Clientes', clienteMapper(cliente));

  await AsyncStorageSetObject('client', data);
};

export const getAddressFromStorage = async () => {
  const {
    usuario: { enderecos },
  } = await AsyncStorageGetObject('client');

  return enderecos[0];
};

export const putEndereco = async endereco => {
  const HttpClient = await HttpClientFactory();

  await HttpClient.put(`Enderecos/${endereco.id}`, endereco);

  await updateAddressOnStorage(endereco);
};
