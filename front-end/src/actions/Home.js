import { action, success } from './index';
import { genericFeedback } from './Feedback';
import { getProdutos, isQuantityValid } from '../services/Home';

export const types = {
  ADD_TO_CART: '[HOME] ADD_TO_CART',
  CHANGE_QUANTITY: '[HOME] CHANGE_QUANTITY',
  FILTER_PRODUCTS: '[HOME] FILTER_PRODUCTS',
  LIST_PRODUCTS: '[HOME] LIST_PRODUCTS',
};

const addToCartAction = ({
  id,
  imagem,
  nome,
  preco,
  quantidade,
}) => dispatch => {
  try {
    dispatch(
      action(types.ADD_TO_CART, {
        idProduto: id,
        imagem,
        nome,
        valor: preco,
        quantidade,
      })
    );
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const changeQuantityAction = (id, quantidade) => dispatch => {
  try {
    if (isQuantityValid(quantidade))
      dispatch(action(types.CHANGE_QUANTITY, { id, quantidade }));
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const filterProductsAction = filtro => dispatch => {
  try {
    dispatch(action(types.FILTER_PRODUCTS, filtro));
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const listProductsAction = (categoria = null) => async dispatch => {
  try {
    dispatch(
      action(success(types.LIST_PRODUCTS), await getProdutos(categoria))
    );
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

export const actions = {
  addToCartAction,
  changeQuantityAction,
  filterProductsAction,
  listProductsAction,
};
