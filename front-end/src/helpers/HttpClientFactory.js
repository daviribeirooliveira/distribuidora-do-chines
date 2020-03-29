/* eslint-disable prefer-promise-reject-errors */
import Axios from 'axios';

import ENV from './Environments';

const { baseURL } = ENV;

const STATUS_CODE = {
  Ok: 200,
  Created: 201,
  NoContent: 204,
  MultipleChoices: 300,
  MovedPermanently: 301,
  Found: 302,
  BadRequest: 400,
  Unauthorized: 401,
  Forbidden: 403,
  NotFound: 404,
  InternalServerError: 500,
  NotImplemented: 501,
  ServiceUnavailable: 503,
};

const HttpClientFactory = async () => {
  const HttpClient = Axios.create({
    baseURL,
    timeout: 10000,
    headers: {
      Authorization: 'Basic QXBwOjFhMDIyNzllNWNlNjQxMjE4NDg5ZmFiMTNkZDlmZTEy',
      'Content-Type': 'application/json',
    },
    validateStatus: status => status < STATUS_CODE.Found,
  });

  HttpClient.interceptors.response.use(
    response => {
      return Promise.resolve(response);
    },
    error => {
      const { status, data } = error.response;

      if (status === STATUS_CODE.BadRequest && typeof data === 'object') {
        const key = Object.keys(error.response.data)[0];

        return Promise.reject({
          message: error.response.data[key][0],
          response: error.response.data,
          status: error.response.status,
        });
      }
      if (status === STATUS_CODE.BadRequest || STATUS_CODE.Forbidden) {
        return Promise.reject({
          message: error.response.data ? error.response.data : '',
          response: error.response ? error.response : '',
          status: error.response.status ? error.response.status : '',
        });
      }
      return Promise.reject({
        message: error.response.data ? error : '',
        response: error.response ? error.response : '',
        status: error.response.status ? error.response.status : '',
      });
    }
  );

  return HttpClient;
};

export default HttpClientFactory;
