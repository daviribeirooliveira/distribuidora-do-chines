import { action, success } from './index';
import { AsyncStorageGetObject } from '../helpers/AsyncStorageExtensions';

import {
  getShippingPrice,
  isCartEmpty,
  isChangeLessThanCartAmount,
  isShippingUnavailable,
  postPedido,
} from '../services/Checkout';

import {
  changeLessThanCartAmountFeedback,
  emptyCartFeedback,
  genericFeedback,
  loadingOnFeedback,
  orderSuccessFeedback,
  unavailabilityFeedback,
} from './Feedback';

export const types = {
  CHANGE_PAYMENT_METHOD: '[CHECKOUT] CHANGE_PAYMENT_METHOD',
  GET_SHIPPING_PRICE: '[CHECKOUT] GET_SHIPPING_PRICE',
  GET_CLIENTE: '[USUARIO] GET_CLIENTE',
  INIT_VALUES: '[CHECKOUT] INIT_VALUES',
  REMOVE_FROM_CART: '[CHECKOUT] REMOVE_FROM_CART',
  SEND_ORDER: '[CHECKOUT] SEND_ORDER',
  UPDATE_CHANGE: '[CHECKOUT] UPDATE_CHANGE',
};

const changePaymentMethodAction = idTiposDePagamento => dispatch => {
  try {
    dispatch(action(types.CHANGE_PAYMENT_METHOD, { idTiposDePagamento }));
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const getClientFromStorage = () => async dispatch => {
  try {
    const { enderecos } = await AsyncStorageGetObject('client');

    dispatch(action(types.GET_CLIENTE, ...enderecos));
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const getShippingPriceAction = () => async dispatch => {
  try {
    dispatch(
      action(success(types.GET_SHIPPING_PRICE), await getShippingPrice())
    );
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const initValuesAction = () => dispatch => {
  try {
    dispatch(action(types.INIT_VALUES, null));
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const removeFromCartAction = idProduto => dispatch => {
  try {
    dispatch(action(types.REMOVE_FROM_CART, idProduto));
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const sendOrderAction = (pedido, callback) => async dispatch => {
  try {
    loadingOnFeedback(dispatch);

    if (isShippingUnavailable(pedido)) {
      unavailabilityFeedback(dispatch);
      return;
    }

    if (isCartEmpty(pedido, dispatch)) {
      emptyCartFeedback(dispatch);
      return;
    }

    if (isChangeLessThanCartAmount(pedido, dispatch)) {
      changeLessThanCartAmountFeedback(dispatch);
      return;
    }

    await postPedido(pedido);

    orderSuccessFeedback(dispatch, callback, action, types);
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const updateChangeAction = troco => dispatch => {
  try {
    dispatch(action(types.UPDATE_CHANGE, { troco }));
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

export const actions = {
  changePaymentMethodAction,
  getShippingPriceAction,
  getClientFromStorage,
  initValuesAction,
  removeFromCartAction,
  sendOrderAction,
  updateChangeAction,
};
