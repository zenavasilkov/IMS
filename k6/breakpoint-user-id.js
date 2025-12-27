import http from 'k6/http';
import { check, sleep } from 'k6';

const CLIENT_ID = __ENV.AUTH0_CLIENT_ID;
const CLIENT_SECRET = __ENV.AUTH0_CLIENT_SECRET;
const AUDIENCE = __ENV.AUTH0_AUDIENCE;
const DOMAIN = __ENV.AUTH0_DOMAIN;

const BASE_URL = 'http://presentation:8080/api'; 

export const options = {
  stages: [
    { duration: '1m', target: 50 },
    { duration: '2m', target: 500 },
    { duration: '2m', target: 2000 },
  ],
  thresholds: {
    http_req_failed: [{ threshold: 'rate<0.01', abortOnFail: true }],
    http_req_duration: [{ threshold: 'p(95)<2000', abortOnFail: true }], 
  },
};

export function setup() {
  const authUrl = `https://${DOMAIN}/oauth/token`;
  const authPayload = JSON.stringify({
    client_id: CLIENT_ID,
    client_secret: CLIENT_SECRET,
    audience: AUDIENCE,
    grant_type: 'client_credentials',
  });
  
  const authHeaders = { headers: { 'Content-Type': 'application/json' } };
  const authRes = http.post(authUrl, authPayload, authHeaders);

  if (authRes.status !== 200) {
    throw new Error(`Auth failed: ${authRes.status} ${authRes.body}`);
  }
  const token = authRes.json().access_token;
  const userHeaders = { headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' } };
  const listUrl = `${BASE_URL}/users?PageNumber=1&PageSize=1`;
  const listRes = http.get(listUrl, userHeaders);
  
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
  const { authToken, userId } = data;

  const params = {
    headers: {
      'Authorization': `Bearer ${authToken}`,
      'Content-Type': 'application/json',
    },
  };

  const res = http.get(`${BASE_URL}/users/${userId}`, params);

  check(res, {
    'status is 200': (r) => r.status === 200,
  });

  sleep(1);
}
