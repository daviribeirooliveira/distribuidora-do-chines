import HttpClientFactory from '../helpers/HttpClientFactory';

export const getCategorias = async () => {
  const HttpClient = await HttpClientFactory();

  const { data } = await HttpClient.get('Categorias');

  return data;
};

export const getProdutos = async categoria => {
  const HttpClient = await HttpClientFactory();

  const { data } = await HttpClient.get(`Produtos?idCategoria=${categoria}`);

  return data;
};

export const isQuantityValid = quantidade =>
  quantidade >= 1 && quantidade <= 999;
