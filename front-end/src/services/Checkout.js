import { AsyncStorageGetObject } from '../helpers/AsyncStorageExtensions';
import HttpClientFactory from '../helpers/HttpClientFactory';

export const addClientToOrder = async pedido => {
  const usuario = await AsyncStorageGetObject('client');

  pedido.idCliente = usuario.id;
  pedido.idClienteEndereco = usuario.enderecos[0].id;

  return pedido;
};

export const postPedido = async pedido => {
  const HttpClient = await HttpClientFactory();

  const order = await addClientToOrder(pedido);

  await HttpClient.post('Pedidos', order);
};

export const getShippingPrice = async () => {
  const HttpClient = await HttpClientFactory();

  const { enderecos } = await AsyncStorageGetObject('client');

  const { data } = await HttpClient.get(
    `Delivery?IdEndereco=${enderecos[0].id}`
  );

  return data;
};

export const isCartEmpty = ({ detalhes }) => detalhes.length === 0;

export const isChangeLessThanCartAmount = ({ troco, valor }) => {
  if (troco) return troco !== 0 && troco < valor;
  return false;
};

export const isShippingUnavailable = pedido => !pedido.disponibilidade;
