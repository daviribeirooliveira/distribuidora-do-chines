/* eslint-disable react/prefer-stateless-function */
import {
  createAppContainer,
  createDrawerNavigator,
  createStackNavigator,
  createSwitchNavigator,
} from 'react-navigation';

import React, { Component } from 'react';

import {
  Cadastro,
  Checkout,
  Endereco,
  Home,
  Login,
  Recuperacao,
} from '../screens';

import Options from './NavigationOptions';
import Splash from './SplashScreen';

const Auth = createStackNavigator({
  Login: { screen: Login, navigationOptions: { headerBackTitle: 'Voltar' } },
  Cadastro: { screen: Cadastro },
  Recuperacao: { screen: Recuperacao },
});

const HomeStack = categorias => {
  categorias.unshift({
    id: null,
    descricao: 'TODOS',
  });

  return Object.assign(
    ...categorias.map(c => ({
      [c.descricao]: { screen: () => <Home categoria={c.id} /> },
    }))
  );
};

const Main = categorias =>
  createStackNavigator({
    HomeStack: createDrawerNavigator(HomeStack(categorias), {
      navigationOptions: Options,
    }),
    Checkout,
    Endereco,
  });

const AppNavigator = categorias =>
  createAppContainer(
    createSwitchNavigator(
      {
        Splash,
        Auth,
        Main: Main(categorias),
      },
      { initialRouteName: 'Splash' }
    )
  );

class NavigatorWrapper extends Component {
  render() {
    const { categorias, ...rest } = this.props;
    return React.createElement(AppNavigator(categorias), rest);
  }
}

export default NavigatorWrapper;
