import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Dimensions, Image, KeyboardAvoidingView } from 'react-native';
import React from 'react';

import { actions } from '../actions/Cliente';
import { AsyncStorageGetObject } from '../helpers/AsyncStorageExtensions';
import { Color } from '../constants/Theme';
import Bloco from '../components/Bloco';
import Botao from '../components/Botao';
import images from '../constants/Images';
import Input from '../components/Input';
import Texto from '../components/Texto';

const { width: WIDTH } = Dimensions.get('window');

class Login extends React.Component {
  static navigationOptions = {
    headerStyle: {
      backgroundColor: Color.main,
      height: 50,
    },
    headerTintColor: Color.secundary,
  };

  constructor() {
    super();
    this.state = {
      email: '',
      senha: '',
    };
  }

  componentWillMount() {
    this.verificaLogin();
  }

  verificaLogin = async () => {
    const { navigation } = this.props;

    const usuario = await AsyncStorageGetObject('client');

    if (usuario) navigation.navigate('Main');
  };

  render() {
    const { signUpAction, navigation } = this.props;

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
              onChangeText={email => this.setState({ email })}
              width={0.8}
            />
            <Input
              label="Senha"
              onChangeText={senha => this.setState({ senha })}
              password
              width={0.8}
              style={{ marginBottom: 20 }}
            />
            <Texto
              sublinhado
              italico
              color={Color.link}
              onPress={() => navigation.navigate('Recuperacao')}
            >
              Esqueci a senha
            </Texto>
            <Bloco style={{ marginTop: 20 }}>
              <Botao
                style={{ marginBottom: 20 }}
                width={0.6}
                onPress={() =>
                  signUpAction({ ...this.state }, () =>
                    navigation.navigate('Main')
                  )
                }
              >
                Login
              </Botao>
              <Texto
                style={{ marginTop: 20 }}
                color={Color.text}
                onPress={() => navigation.navigate('Cadastro')}
              >
                Não tem uma conta?{' '}
                <Texto
                  sublinhado
                  italico
                  color={Color.link}
                  style={{ marginTop: 20 }}
                >
                  Cadastre-se
                </Texto>
              </Texto>
            </Bloco>
          </Bloco>
        </Bloco>
      </KeyboardAvoidingView>
    );
  }
}

const mapActionsToProps = dispatch => bindActionCreators(actions, dispatch);

export default connect(null, mapActionsToProps)(Login);
