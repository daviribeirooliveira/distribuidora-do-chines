import { Provider } from 'react-redux';
import { StatusBar } from 'react-native';
import AwesomeAlert from 'react-native-awesome-alerts';
import React from 'react';

import { cacheImages, cacheFonts } from './src/helpers/CacheAssets';
import { Color } from './src/constants/Theme';
import { getCategorias } from './src/services/Home';
import AlertConnected from './src/components/Alert';
import Bloco from './src/components/Bloco';
import ConfigureStore from './ConfigureStore';
import Fonts from './src/constants/Fonts';
import Images from './src/constants/Images';
import NavigationService from './src/navigation/NavigationService';
import NavigatorWrapper from './src/navigation/AppNavigator';
import SplashLogo from './src/components/SplashLogo';

const store = ConfigureStore();

export default class App extends React.Component {
  constructor() {
    super();
    this.state = {
      categorias: [],
    };
  }

  componentDidMount() {
    this.cacheAssets();
  }

  cacheData = async () => {
    const categorias = await getCategorias();

    this.setState(prevState => ({ ...prevState, categorias }));
  };

  startApp = async () => {
    this.setState(prevState => ({ ...prevState, error: false, ready: true }));
  };

  cacheAssets = async () => {
    try {
      const FontsPromise = cacheImages(Object.values(Images));
      const ImagesPromise = cacheFonts(Fonts);
      const DataPromise = this.cacheData();

      await Promise.all([...FontsPromise, ...ImagesPromise, DataPromise]);

      this.startApp();
    } catch (error) {
      this.setState({ error: true });
    }
  };

  render() {
    const { categorias, error, ready } = this.state;

    return !ready ? (
      <Bloco>
        <SplashLogo />
        <AwesomeAlert
          show={error}
          showProgress={false}
          title="Distribuidora do Chinês"
          message="Não foi possível iniciar o app, verifique sua conexão!"
          closeOnTouchOutside={false}
          closeOnHardwareBackPress={false}
          showConfirmButton
          confirmText="Recarregar"
          confirmButtonColor={Color.main}
          onConfirmPressed={this.cacheAssets}
        />
      </Bloco>
    ) : (
      <Provider store={store}>
        <StatusBar backgroundColor="black" barStyle="light-content" />
        <Bloco>
          <NavigatorWrapper
            categorias={categorias}
            ref={navigatorRef => {
              NavigationService.setTopLevelNavigator(navigatorRef);
            }}
          />
          <AlertConnected />
        </Bloco>
      </Provider>
    );
  }
}
