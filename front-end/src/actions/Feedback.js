export function feedback({
  closeOnHardwareBackPress,
  closeOnTouchOutside,
  confirmText,
  message,
  onConfirmPressed,
  show,
  showProgress,
  title,
  type,
}) {
  return {
    type: 'FEEDBACK',
    payload: {
      closeOnHardwareBackPress,
      closeOnTouchOutside,
      confirmText,
      message,
      onConfirmPressed,
      show,
      showProgress,
      title,
      type,
    },
  };
}

export function loadingOnFeedback(dispatch) {
  dispatch(
    feedback({
      message: 'Carregando...',
      showProgress: true,
    })
  );
}

export function loadingOffFeedback(dispatch, callback) {
  dispatch({ type: 'MODAL_CLOSE' });

  if (callback) callback();
}

export function genericFeedback(dispatch, error) {
  dispatch(
    feedback({
      confirmText: 'Ok',
      type: 'error',
      message:
        error.status === 400 || error.status === 401
          ? error.message
          : 'Ocorreu um erro inesperado, por favor, entre em contato conosco.',
    })
  );
}

export function emptyCartFeedback(dispatch) {
  dispatch(
    feedback({
      confirmText: 'Ok',
      type: 'info',
      message: 'Carrinho vazio!, por favor adicione itens ao seu carrinho.',
    })
  );
}

export function changeLessThanCartAmountFeedback(dispatch) {
  dispatch(
    feedback({
      confirmText: 'Ok',
      type: 'info',
      message: 'Troco deve ser maior que o total de pedido!',
    })
  );
}

export function orderSuccessFeedback(dispatch, callback, action, types) {
  dispatch(
    feedback({
      confirmText: 'Ok',
      type: 'info',
      message: 'Pedido efetuado com success!',
      onConfirmPressed: callback,
    })
  );
  dispatch(action(types.SEND_ORDER));
}

export function unavailabilityFeedback(dispatch) {
  dispatch(
    feedback({
      confirmText: 'Ok',
      type: 'info',
      message: 'Ainda não temos disponibilidade de entrega para sua região :(',
    })
  );
}

export function addressUpdateFeedback(dispatch, callback) {
  dispatch(
    feedback({
      confirmText: 'Voltar',
      message: 'Endereço atualizado com success!',
      onConfirmPressed: callback,
    })
  );
}

export function signInFeedback(dispatch, callback) {
  dispatch(
    feedback({
      confirmText: 'Login',
      message: 'Cadastro realizado com success!',
      onConfirmPressed: callback,
    })
  );
}

export function passwordRecoveryFeedback(dispatch, callback) {
  dispatch(
    feedback({
      confirmText: 'Voltar',
      message: 'Recuperação enviada com sucesso, verifique seu email.',
      onConfirmPressed: callback,
    })
  );
}
