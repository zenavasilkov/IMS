import http from 'k6/http';

export const config = {
  auth: {
    clientId: __ENV.AUTH0_CLIENT_ID,
    clientSecret: __ENV.AUTH0_CLIENT_SECRET,
    audience: __ENV.AUTH0_AUDIENCE,
    domain: __ENV.AUTH0_DOMAIN,
  },
  services: {
    recruitment: 'http://recruitmentservice:8080/api',
    presentation: 'http://presentation:8080/api',
  }
};

export function authenticate() {
  const url = `https://${config.auth.domain}/oauth/token`;
  
  const payload = JSON.stringify({
    client_id: config.auth.clientId,
    client_secret: config.auth.clientSecret,
    audience: config.auth.audience,
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

  return res.json().access_token;
}

export function getParams(token) {
  return {
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
  };
}
