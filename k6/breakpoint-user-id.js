import http from 'k6/http';
import { check, sleep } from 'k6';
import { authenticate, getParams, config } from './utils.js';

export const options = {
  stages: [
    { duration: '30s', target: 50 },
    { duration: '2m', target: 500 },
    { duration: '2m', target: 2000 },
  ],
  thresholds: {
    http_req_failed: [{ threshold: 'rate<0.01', abortOnFail: true }],
    http_req_duration: [{ threshold: 'p(95)<2000', abortOnFail: true }], 
  },
};

export function setup() {
  const token = authenticate();
  const params = getParams(token);
  const listUrl = `${config.services.presentation}/users?PageNumber=1&PageSize=1`;
  const listRes = http.get(listUrl, params);
  
  if (listRes.status !== 200) {
    throw new Error(`Failed to fetch users list: ${listRes.status}`);
  }

  const jsonResponse = listRes.json();
  
  if (!jsonResponse.items || jsonResponse.items.length === 0) {
    throw new Error('No users found in database to test with!');
  }

  const targetUserId = jsonResponse.items[0].id;
  console.log(`Setup successful! Target User ID: ${targetUserId}`);

  return { authToken: token, userId: targetUserId };
}

export default function (data) {
  const params = getParams(data.authToken);
  const res = http.get(`${config.services.presentation}/users/${data.userId}`, params);

  check(res, {'status is 200': (r) => r.status === 200});

  sleep(1);
}
