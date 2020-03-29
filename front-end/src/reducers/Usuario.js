import { types } from '../actions/Usuario';

const Usuario = (state = {}, { type }) => {
  switch (type) {
    case types.LOGIN:
      return { ...state };

    default:
      return state;
  }
};

export default Usuario;
