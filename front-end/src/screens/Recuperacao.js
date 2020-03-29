import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Dimensions, Image, KeyboardAvoidingView } from 'react-native';
import React from 'react';

import { actions } from '../actions/Cliente';
import { Color } from '../constants/Theme';
import images from '../constants/Images';

import Bloco from '../components/Bloco';
import Botao from '../components/Botao';
import Input from '../components/Input';

const { width: WIDTH } = Dimensions.get('window');

class Recuperacao extends React.Component {
  static navigationOptions = {
    headerBackTitle: 'Voltar',
    headerStyle: {
      backgroundColor: Color.main,
      height: 50,
    },
    headerTintColor: Color.secundary,
  };

  state = {
    email: '',
  };

  render() {
    const { passwordRecoveryAction, navigation } = this.props;
    const { email } = this.state;

    return (
      <KeyboardAvoidingView enabled behavior="padding" style={{ flex: 1 }}>
        <Bloco center middle background={Color.background}>
          <Bloco middle>
            <Image
              resizeMode="contain"
              source={images.logo}
              style={{ width: WIDTH * 0.8 }}
            />
          </Bloco>
          <Bloco center flex={2.5}>
            <Input
              label="E-mail"
              type="email-address"
              width={0.8}
              onChangeText={input => this.setState({ email: input })}
            />
            <Bloco style={{ marginTop: 12 }}>
              <Botao
                style={{ marginBottom: 12 }}
                width={0.6}
                onPress={() =>
                  passwordRecoveryAction(email, () =>
                    navigation.navigate('Login')
                  )
                }
              >
                Recuperar
              </Botao>
            </Bloco>
          </Bloco>
        </Bloco>
      </KeyboardAvoidingView>
    );
  }
}

const mapActionsToProps = dispatch => bindActionCreators(actions, dispatch);

export default connect(null, mapActionsToProps)(Recuperacao);
