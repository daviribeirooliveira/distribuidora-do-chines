/* eslint-disable global-require */
import { Ionicons } from '@expo/vector-icons';

const fonts = [
  {
    Roboto: require('../../assets/fonts/Roboto.ttf'),
    Roboto_medium: require('../../assets/fonts/Roboto_medium.ttf'),
    ...Ionicons.font,
  },
];

export default fonts;
