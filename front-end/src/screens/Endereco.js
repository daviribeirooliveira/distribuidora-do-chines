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

class Endereco extends React.Component {
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
      bairro: '',
      cep: '',
      complemento: '',
      id: '',
      nomeEndereco: '',
      numero: '',
      referencia: '',
      rua: '',
      telefone: '',
    };
  }

  componentDidMount() {
    const { navigation } = this.props;

    const {
      state: {
        params: { endereco },
      },
    } = navigation;

    this.setState(prevState => ({ ...prevState, ...endereco }));
  }

  render() {
    const { navigation, updateAddressAction } = this.props;

    const { bairro, cep, complemento, numero, referencia, rua } = this.state;

    return (
      <KeyboardShift>
        {() => (
          <Bloco center background="#fff200">
            <Input
              label="Cep"
              defaultValue={cep}
              placeholder="Cep. Ex: 60025902"
              width={0.8}
              type="numeric"
              onChangeText={input => this.setState({ cep: input })}
            />
            <Input
              label="Rua"
              defaultValue={rua}
              placeholder="Rua. Ex: José de abreu"
              width={0.8}
              autoCapitalize="words"
              onChangeText={input => this.setState({ rua: input })}
            />
            <Input
              label="Nº"
              defaultValue={numero}
              placeholder="Nº. Ex: 429"
              width={0.8}
              type="numeric"
              onChangeText={input => this.setState({ numero: input })}
            />
            <Input
              label="Bairro"
              defaultValue={bairro}
              placeholder="Bairro. Ex: Centro"
              width={0.8}
              autoCapitalize="words"
              onChangeText={input => this.setState({ bairro: input })}
            />
            <Input
              label="Complemento"
              defaultValue={complemento}
              placeholder="Complemento. Ex: Bloco A, 101"
              width={0.8}
              autoCapitalize="sentences"
              onChangeText={input => this.setState({ complemento: input })}
            />
            <Input
              label="Ponto de referência"
              defaultValue={referencia}
              placeholder="Referência. Ex: Na rua do mercado"
              width={0.8}
              autoCapitalize="sentences"
              onChangeText={input => this.setState({ referencia: input })}
            />
            <Bloco style={Styles.btnView}>
              <Botao
                width={0.4}
                onPress={() =>
                  updateAddressAction(this.state, () =>
                    navigation.navigate('Checkout')
                  )
                }
              >
                Atualizar
              </Botao>
            </Bloco>
          </Bloco>
        )}
      </KeyboardShift>
    );
  }
}

const mapActionsToProps = dispatch => bindActionCreators(actions, dispatch);

export default connect(null, mapActionsToProps)(Endereco);
