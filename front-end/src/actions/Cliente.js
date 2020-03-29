import {
  addressUpdateFeedback,
  genericFeedback,
  loadingOnFeedback,
  loadingOffFeedback,
  signInFeedback,
  passwordRecoveryFeedback,
} from './Feedback';

import {
  appAuthenticate,
  passwordRecovery,
  postCliente,
  putEndereco,
} from '../services/Cliente';

export const types = {
  SIGN_UP: '[USUARIO] SIGN_UP',
  UPDATE_ADDRESS: '[USUARIO] UPDATE_ADDRESS',
  LOGIN: '[USUARIO] LOGIN',
};

const updateAddressAction = (endereco, callback) => async dispatch => {
  try {
    loadingOnFeedback(dispatch);

    await putEndereco(endereco);

    addressUpdateFeedback(dispatch, callback);
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const signInAction = (cliente, callback) => async dispatch => {
  try {
    loadingOnFeedback(dispatch);

    await postCliente(cliente);

    signInFeedback(dispatch, callback);
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const signUpAction = (cliente, callback) => async dispatch => {
  try {
    loadingOnFeedback(dispatch);

    await appAuthenticate(cliente);

    loadingOffFeedback(dispatch, callback);
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

const passwordRecoveryAction = (email, callback) => async dispatch => {
  try {
    loadingOnFeedback(dispatch);

    await passwordRecovery(email);

    passwordRecoveryFeedback(dispatch, callback);
  } catch (error) {
    genericFeedback(dispatch, error);
  }
};

export const actions = {
  passwordRecoveryAction,
  signInAction,
  signUpAction,
  updateAddressAction,
};
