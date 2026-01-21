import http from 'k6/http';
import { check, sleep } from 'k6';
import { authenticate, getParams, config } from './utils.js';

export const options = {
  stages: [
    { duration: '30s', target: 200 },
    { duration: '1m', target: 200 },
    { duration: '10s', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(95)<2000'],
    http_req_failed: ['rate<0.01'], 
  },
};

export function setup() {
  const token = authenticate();
  return { authToken: token };
}

export default function (data) {
  const params = getParams(data.authToken);
  
  const res = http.get(`${config.services.recruitment}/candidates/get-all?PaginationParameters.PageNumber=1&PaginationParameters.PageSize=10`, params);

  check(res, {'status is 200': (r) => r.status === 200});

  sleep(Math.random() * 1 + 0.5);
}
