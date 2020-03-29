/* eslint-disable no-shadow */
export const action = (action, payload = null) => ({
  type: `${action}`,
  payload,
});

export const success = action => `${action} => SUCESSO`;

export const falha = action => `${action} => FALHA`;
