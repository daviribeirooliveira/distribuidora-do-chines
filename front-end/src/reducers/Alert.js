const Alert = (state = {}, { type, payload }) => {
  switch (type) {
    case 'FEEDBACK':
      return { ...payload, show: true };

    case 'MODAL_CLOSE':
      return { ...payload, show: false };

    default:
      return state;
  }
};

export default Alert;
