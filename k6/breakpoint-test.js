import http from 'k6/http';
import { check, sleep } from 'k6';

const CLIENT_ID = __ENV.AUTH0_CLIENT_ID;
const CLIENT_SECRET = __ENV.AUTH0_CLIENT_SECRET;
const AUDIENCE = __ENV.AUTH0_AUDIENCE;
const DOMAIN = __ENV.AUTH0_DOMAIN;

const BASE_URL = 'http://recruitmentservice:8080/api'; 

export const options = {
  stages: [
    { duration: '2m', target: 500 },
    { duration: '2m', target: 1000 },
  ],
  thresholds: {
    http_req_failed: [{ threshold: 'rate<0.01', abortOnFail: true }],
    http_req_duration: [{ threshold: 'p(95)<3000', abortOnFail: true }], 
  },
};

export function setup() {
  const url = `https://${DOMAIN}/oauth/token`;
  
  const payload = JSON.stringify({
    client_id: CLIENT_ID,
    client_secret: CLIENT_SECRET,
    audience: AUDIENCE,
    grant_type: 'client_credentials',
  });

  const params = {
    headers: { 'Content-Type': 'application/json' },
  };

  const res = http.post(url, payload, params);

  if (res.status !== 200) {
    console.error(`Auth failed: ${res.status} ${res.body}`);
    throw new Error(`Auth failed: ${res.status}`);
  }

  return { authToken: res.json().access_token };
}

export default function (data) {
  const params = {
    headers: {
      'Authorization': `Bearer ${data.authToken}`,
      'Content-Type': 'application/json',
    },
  };

  const res = http.get(`${BASE_URL}/candidates/get-all?PaginationParameters.PageNumber=1&PaginationParameters.PageSize=10`, params);

  check(res, {
    'status is 200': (r) => r.status === 200,
  });

  sleep(1);
}
