import Constants from 'expo-constants';

// const devURL = 'http://192.168.100.105:5000/';
const prodURL = 'http://api.distribuidoradochines.com/';

const ENV = {
  dev: {
    baseURL: prodURL,
  },
  staging: {
    baseURL: prodURL,
  },
  prod: {
    baseURL: prodURL,
  },
};

function getEnv(env = '') {
  if (env === null || env === undefined || env === '') return ENV.dev;
  if (env.indexOf('dev') !== -1) return ENV.dev;
  if (env.indexOf('staging') !== -1) return ENV.staging;
  if (env.indexOf('prod') !== -1) return ENV.prod;

  throw new Error('Error getting env variables');
}

export default getEnv(Constants.manifest.releaseChannel);
