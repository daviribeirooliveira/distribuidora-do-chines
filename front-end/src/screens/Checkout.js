import { bindActionCreators } from 'redux';
import {
  Body,
  Container,
  Content,
  Footer,
  FooterTab,
  Header,
  Icon,
  Input,
  Left,
  Picker,
  Right,
  Text,
  Title,
  StyleProvider,
} from 'native-base';
import { connect } from 'react-redux';
import { View, Dimensions, FlatList, Platform } from 'react-native';
import React from 'react';
import { withNavigation } from 'react-navigation';

import { actions } from '../actions/Checkout';
import { Color } from '../constants/Theme';
import Botao from '../components/Botao';
import CheckoutCard from '../components/CheckoutCard';
import getTheme from '../../native-base-theme/components';
import customVariables from '../../native-base-theme/variables/variables';

const { height: HEIGHT } = Dimensions.get('window');

class Checkout extends React.Component {
  static navigationOptions = {
    headerStyle: {
      backgroundColor: Color.main,
      height: 50,
    },
    headerTintColor: Color.secundary,
    headerBackTitle: 'Voltar',
  };

  constructor() {
    super();
    this.state = {
      trocoEnabled: true,
      pedidoEnabled: false,
    };
  }

  componentWillMount() {
    const {
      getShippingPriceAction,
      getClientFromStorage,
      initValuesAction,
      navigation,
    } = this.props;

    initValuesAction();

    navigation.addListener('didFocus', () => {
      getClientFromStorage();
      getShippingPriceAction();
      this.getCoffee();
    });
  }

  onValueChange = value => {
    const { changePaymentMethodAction } = this.props;

    changePaymentMethodAction(value);

    if (value !== '1') {
      this.setState(prevState => ({
        ...prevState,
        troco: '',
        trocoEnabled: false,
      }));
    } else {
      this.setState(prevState => ({
        ...prevState,
        troco: '',
        trocoEnabled: true,
      }));
    }
  };

  onTextChange = value => {
    const { updateChangeAction } = this.props;

    this.setState(prevState => ({ ...prevState, troco: value }));
    updateChangeAction(value);
  };

  getCoffee = () => {
    setTimeout(
      () => this.setState(prevState => ({ ...prevState, pedidoEnabled: true })),
      1000
    );
  };

  render() {
    const { navigation, stateCheckout, sendOrderAction } = this.props;
    const { endereco } = stateCheckout;
    const { rua, numero, bairro, complemento, referencia } = endereco;
    const { pedidoEnabled, troco, trocoEnabled } = this.state;

    return (
      <Container>
        <StyleProvider style={getTheme(customVariables)}>
          <Header
            style={{
              height: 150,
              backgroundColor: '#fff',
            }}
          >
            <View
              style={{
                alignItems: 'center',
              }}
            >
              <Text
                style={{
                  fontSize: 16,
                  fontWeight: 'bold',
                  marginBottom: 5,
                }}
              >
                Endereço de Entrega
              </Text>
              <Text
                style={{ fontSize: 12, fontWeight: 'bold', color: '#bdc3c7' }}
              >
                {rua.trim()}
                {' N° '}
                {numero.trim()}
              </Text>
              <Text
                style={{ fontSize: 12, fontWeight: 'bold', color: '#bdc3c7' }}
              >
                {bairro.trim()}
              </Text>
              <Text
                style={{
                  fontSize: 12,
                  fontWeight: 'bold',
                  color: '#bdc3c7',
                  marginBottom: 10,
                }}
              >
                {complemento?.trim()}
                {' - '}
                {referencia?.trim()}
              </Text>
              <Botao
                width={0.4}
                onPress={() => navigation.navigate('Endereco', { endereco })}
              >
                <Text style={{ color: Color.secundary, marginBottom: 10 }}>
                  Editar endereço
                </Text>
              </Botao>
            </View>
          </Header>
        </StyleProvider>
        <Content>
          <FlatList
            keyExtractor={item => item.idProduto.toString()}
            data={stateCheckout.detalhes || null}
            renderItem={({ item }) => <CheckoutCard produto={item} />}
          />
        </Content>
        <StyleProvider style={getTheme(customVariables)}>
          <Footer style={{ height: HEIGHT * 0.25 }}>
            <FooterTab
              style={{
                flex: 1,
                flexDirection: 'column',
              }}
            >
              <FooterTab style={{ flex: 0.4, marginTop: 10 }}>
                <Left style={{ flex: 0.5 }}>
                  <Text style={{ flex: 0.5, marginLeft: 15 }}>
                    Taxa de entrega:
                  </Text>
                  <Text style={{ flex: 0.5, marginLeft: 15 }}>
                    Total do pedido:{' '}
                  </Text>
                </Left>
                <Right style={{ flex: 0.5 }}>
                  <Text style={{ flex: 0.5, marginRight: 15 }}>
                    R$:
                    {stateCheckout.valorFrete
                      ? stateCheckout.valorFrete.toFixed(2)
                      : '0.00'}
                  </Text>
                  <Text
                    style={{
                      flex: 0.5,
                      marginRight: 15,
                      fontWeight: 'bold',
                      color: Color.main,
                    }}
                  >
                    R$:
                    {stateCheckout.valor
                      ? stateCheckout.valor.toFixed(2)
                      : '0.00'}
                  </Text>
                </Right>
              </FooterTab>
              <FooterTab
                style={{
                  flex: 0.33,
                  marginLeft: Platform.OS === 'ios' ? 0 : 9,
                }}
              >
                <Picker
                  renderHeader={backAction => (
                    <Header style={{ backgroundColor: Color.main }}>
                      <Left>
                        <Botao transparent onPress={backAction}>
                          <Icon name="arrow-back" style={{ color: '#fff' }} />
                        </Botao>
                      </Left>
                      <Body
                        style={{
                          justifyCotent: 'center',
                          alignContent: 'center',
                        }}
                      >
                        <Title style={{ color: '#fff' }}>
                          Forma de Pagamento
                        </Title>
                      </Body>
                    </Header>
                  )}
                  mode="dropdown"
                  placeholder="Selecione a forma de pagamento:"
                  iosIcon={<Icon name="arrow-down" />}
                  headerStyle={{
                    justifyContent: 'center',
                    backgroundColor: Color.main,
                  }}
                  headerBackButtonText="Voltar"
                  selectedValue={stateCheckout.idTiposDePagamento}
                  onValueChange={this.onValueChange}
                >
                  <Picker.Item label="Dinheiro" value="1" />
                  <Picker.Item label="Cartão de crédito" value="2" />
                  <Picker.Item label="Cartão de débito" value="3" />
                </Picker>
              </FooterTab>
              <FooterTab
                style={{
                  flex: 0.33,
                  flexDirection: 'row',
                  marginBottom: 10,
                }}
              >
                <Left>
                  <Input
                    style={{
                      // flexDirection: 'row',
                      // justifyContent: 'flex-start',
                      marginLeft: 10,
                      flex: 0.5,
                    }}
                    type="numeric"
                    disabled={!trocoEnabled}
                    placeholder="Troco? R$:"
                    onChangeText={value => this.onTextChange(value)}
                    value={troco}
                  />
                </Left>
                <Body>
                  <Botao
                    style={{}}
                    disabled={!pedidoEnabled}
                    width={0.4}
                    onPress={async () => sendOrderAction(stateCheckout)}
                  >
                    <Text style={{ color: Color.secundary }}>Fazer pedido</Text>
                  </Botao>
                </Body>
                <Right />
              </FooterTab>
            </FooterTab>
          </Footer>
        </StyleProvider>
      </Container>
    );
  }
}

const mapActionsToProps = dispatch => bindActionCreators(actions, dispatch);

const select = state => ({
  stateCheckout: state.Checkout,
});

export default connect(select, mapActionsToProps)(withNavigation(Checkout));
