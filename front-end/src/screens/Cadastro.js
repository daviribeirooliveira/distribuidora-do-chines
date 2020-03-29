import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { StyleSheet } from 'react-native';
import React from 'react';

import { actions } from '../actions/Cliente';
import { Color } from '../constants/Theme';
import Bloco from '../components/Bloco';
import Botao from '../components/Botao';
import Input from '../components/Input';
import KeyboardShift from '../components/KeyboardShift';

const Styles = StyleSheet.create({
  btnView: {
    marginTop: 10,
  },
});

class Cadastro extends React.Component {
  static navigationOptions = {
    headerStyle: {
      backgroundColor: Color.main,
      height: 50,
    },
    headerTintColor: Color.secundary,
  };

  render() {
    const { navigation, signInAction } = this.props;

    return (
      <KeyboardShift>
        {() => (
          <Bloco center background="#fff200">
            <Input
              label="E-mail."
              width={0.8}
              type="email-address"
              onChangeText={email => this.setState({ email })}
            />
            <Input
              label="Senha."
              width={0.8}
              password
              onChangeText={senha => this.setState({ senha })}
            />
            <Input
              label="Nome completo."
              placeholder="Nome. Ex: Lucas Ribeiro"
              width={0.8}
              autoCapitalize="words"
              onChangeText={nome => this.setState({ nome })}
            />
            <Input
              label="Cep"
              placeholder="Cep. Ex: 60025902"
              width={0.8}
              type="numeric"
              onChangeText={cep => this.setState({ cep })}
            />
            <Input
              label="Rua"
              placeholder="Rua. Ex: José de abreu"
              width={0.8}
              autoCapitalize="words"
              onChangeText={rua => this.setState({ rua })}
            />
            <Input
              label="Nº"
              placeholder="Nº. Ex: 429"
              width={0.8}
              type="numeric"
              onChangeText={numero => this.setState({ numero })}
            />
            <Input
              label="Bairro"
              placeholder="Bairro. Ex: Centro"
              width={0.8}
              autoCapitalize="words"
              onChangeText={bairro => this.setState({ bairro })}
            />
            <Input
              label="Complemento"
              placeholder="Complemento. Ex: Bloco A, 101"
              width={0.8}
              autoCapitalize="sentences"
              onChangeText={complemento => this.setState({ complemento })}
            />
            <Input
              label="Ponto de referência"
              placeholder="Referência. Ex: Na rua do mercado"
              width={0.8}
              autoCapitalize="sentences"
              onChangeText={referencia => this.setState({ referencia })}
            />
            <Input
              label="Telefone"
              placeholder="Celular. Ex: 999999999"
              width={0.8}
              type="phone-pad"
              onChangeText={telefone => this.setState({ telefone })}
            />
            <Bloco style={Styles.btnView}>
              <Botao
                width={0.4}
                onPress={() =>
                  signInAction(this.state, () => navigation.navigate('Main'))
                }
              >
                Cadastrar
              </Botao>
            </Bloco>
          </Bloco>
        )}
      </KeyboardShift>
    );
  }
}

const mapActionsToProps = dispatch => bindActionCreators(actions, dispatch);

export default connect(null, mapActionsToProps)(Cadastro);
