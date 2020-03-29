import { connect } from 'react-redux';
import AwesomeAlert from 'react-native-awesome-alerts';
import React from 'react';

import { Color } from '../constants/Theme';

class Alert extends React.Component {
  onConfirmPressed = () => {
    const { modalClose, stateAlert } = this.props;
    const { onConfirmPressed } = stateAlert;

    if (onConfirmPressed) {
      onConfirmPressed();
    }

    modalClose();
  };

  render() {
    const { stateAlert } = this.props;

    const {
      type,
      show,
      showProgress,
      message,
      closeOnTouchOutside,
      closeOnHardwareBackPress,
      confirmText,
    } = { ...stateAlert };

    return (
      <AwesomeAlert
        closeOnHardwareBackPress={closeOnHardwareBackPress || false}
        closeOnTouchOutside={closeOnTouchOutside || false}
        confirmButtonColor={type === 'error' ? Color.main : Color.success}
        confirmText={confirmText}
        message={message || ''}
        onConfirmPressed={this.onConfirmPressed}
        show={show}
        showConfirmButton={!!confirmText}
        showProgress={showProgress || false}
        title={type === 'error' ? 'Erro!' : 'Distribuidora do Chinês'}
      />
    );
  }
}

const mapActionsToProps = dispatch => ({
  modalClose: () => dispatch({ type: 'MODAL_CLOSE' }),
});

const select = state => ({
  stateAlert: state.Alert,
});

export default connect(select, mapActionsToProps)(Alert);
