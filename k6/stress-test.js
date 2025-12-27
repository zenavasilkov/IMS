import http from 'k6/http';
import { check, sleep } from 'k6';

const CLIENT_ID = __ENV.AUTH0_CLIENT_ID;
const CLIENT_SECRET = __ENV.AUTH0_CLIENT_SECRET;
const AUDIENCE = __ENV.AUTH0_AUDIENCE;
const DOMAIN = __ENV.AUTH0_DOMAIN;

const BASE_URL = 'http://recruitmentservice:8080/api'; 

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

  const token = res.json().access_token;
  return { authToken: token };
}

export default function (data) {
  const token = data.authToken;

  const params = {
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
  };

  const res = http.get(`${BASE_URL}/candidates/get-all?PaginationParameters.PageNumber=1&PaginationParameters.PageSize=10`, params);

  check(res, {
    'status is 200': (r) => r.status === 200,
  });

  sleep(Math.random() * 1 + 0.5);
}
