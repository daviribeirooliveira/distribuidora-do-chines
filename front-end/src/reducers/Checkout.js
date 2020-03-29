/* eslint-disable no-case-declarations */
import { success } from '../actions/index';
import * as CheckoutTypes from '../actions/Checkout';
import * as HomeTypes from '../actions/Home';

const init = {
  idCliente: 0,
  idClienteEndereco: 0,
  idTiposDePagamento: 1,
  valor: 0,
  valorEmReais: 0,
  valorFrete: 0,
  valorFreteEmReais: 0,
  status: 'Novo',
  troco: '',
  disponibilidade: true,
  detalhes: [],
  endereco: {
    rua: '',
    numero: '',
    bairro: '',
    complemento: '',
    referencia: '',
  },
};

const Checkout = (state = init, { type, payload }) => {
  switch (type) {
    case HomeTypes.types.ADD_TO_CART:
      const existingProduct = state.detalhes.find(
        product => product.idProduto === payload.idProduto
      );

      if (existingProduct) existingProduct.quantidade += 1;
      else state.detalhes.push(payload);

      return { ...state };

    case CheckoutTypes.types.ADICIONAR_FORMA_DE_PAGAMENTO:
      return {
        ...state,
        idTiposDePagamento: payload.idTiposDePagamento,
      };

    case CheckoutTypes.types.CHANGE_PAYMENT_METHOD:
      let trocoAtualizado = state.troco;

      if (payload.idTiposDePagamento !== 1) {
        trocoAtualizado = 0;
      }

      return {
        ...state,
        troco: trocoAtualizado,
        idTiposDePagamento: payload.idTiposDePagamento,
      };

    case CheckoutTypes.types.UPDATE_CHANGE:
      return { ...state, troco: payload.troco };

    case success(CheckoutTypes.types.GET_SHIPPING_PRICE):
      const sum = state.detalhes.reduce(
        (accumulator, product) =>
          accumulator + product.valor * product.quantidade,
        0
      );

      return {
        ...state,
        disponibilidade: payload.availability,
        valorFrete: payload.price,
        valor: sum + payload.price,
      };

    case CheckoutTypes.types.SEND_ORDER:
      return {
        ...state,
        detalhes: [],
        valor: state.valorFrete,
      };

    case CheckoutTypes.types.REMOVE_FROM_CART:
      const updatedList = state.detalhes.filter(
        products => products.idProduto !== payload
      );

      const updatedSum = updatedList.reduce(
        (accumulator, product) =>
          accumulator + product.valor * product.quantidade,
        0
      );

      return {
        ...state,
        valor: updatedSum + state.valorFrete,
        detalhes: updatedList,
      };

    case CheckoutTypes.types.GET_CLIENTE:
      return { ...state, endereco: payload };

    default:
      return state;
  }
};

export default Checkout;
