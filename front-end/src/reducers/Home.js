import { success } from '../actions';
import { types } from '../actions/Home';

const changeQuantity = ({ id, quantidade }) => product => {
  if (product.id === id) return { ...product, quantidade };
  return product;
};

const Home = (state = {}, { type, payload }) => {
  switch (type) {
    case success(types.LIST_PRODUCTS):
      return {
        ...state,
        produtos: payload,
        produtosFiltrados: payload.map(p => ({ ...p, quantidade: 1 })),
      };

    case types.FILTER_PRODUCTS:
      return {
        ...state,
        produtosFiltrados: state.produtos
          .filter(p => p.nome.toLowerCase().indexOf(payload.toLowerCase()) > -1)
          .map(p => ({ ...p, quantidade: 1 })),
      };

    case types.CHANGE_QUANTITY:
      return {
        ...state,
        produtosFiltrados: state.produtosFiltrados.map(changeQuantity(payload)),
      };

    default:
      return state;
  }
};

export default Home;
