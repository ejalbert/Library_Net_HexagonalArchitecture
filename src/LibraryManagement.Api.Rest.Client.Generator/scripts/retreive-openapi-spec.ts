import fetch from 'node-fetch';
import http from 'http';
import https from 'https';
import * as fs from 'fs';
import * as path from 'path';
import { exec } from 'child_process';

// Configuration - update this URL to match your LibraryManagement API
const OPENAPI_URL = process.env.OPENAPI_URL ?? 'http://localhost:5007/api/v1.json';
const OUTPUT_FOLDER = './openapi-specs';

// Agents for HTTP and HTTPS (allow self-signed certificates for local dev)
const httpAgent = new http.Agent();
const httpsAgent = new https.Agent({
  rejectUnauthorized: false,
});

const getAgent = (url: URL) => (url.protocol === 'https:' ? httpsAgent : httpAgent);

const downloadJsonFile = async (folder: string) => {
  console.log(`Fetching OpenAPI spec from: ${OPENAPI_URL}`);
  try {
    const url = new URL(OPENAPI_URL);
    const response = await fetch(OPENAPI_URL, {
      agent: getAgent(url),
      redirect: 'manual', // Handle redirects manually to switch agents
    });

    // Handle redirects (HTTP → HTTPS)
    if (response.status >= 300 && response.status < 400 && response.headers.get('location')) {
      const redirectUrl = new URL(response.headers.get('location')!, OPENAPI_URL);
      console.log(`Following redirect to: ${redirectUrl.href}`);
      const redirectResponse = await fetch(redirectUrl.href, {
        agent: getAgent(redirectUrl),
      });
      if (!redirectResponse.ok) {
        throw new Error(`HTTP ${redirectResponse.status}: ${redirectResponse.statusText}`);
      }
      const json = await redirectResponse.json();
      await saveSpec(folder, json);
      return;
    }

    if (!response.ok) {
      throw new Error(`HTTP ${response.status}: ${response.statusText}`);
    }
    const json = await response.json();
    await saveSpec(folder, json);
  } catch (error) {
    console.error(`Error downloading OpenAPI spec from ${OPENAPI_URL}: ${error}`);
    process.exit(1);
  }
};

const saveSpec = async (folder: string, json: unknown) => {
  if (!fs.existsSync(folder)) {
    fs.mkdirSync(folder, { recursive: true });
  }
  const filePath = path.join(folder, 'openapi.json');
  fs.writeFileSync(filePath, JSON.stringify(json, null, 2));
  console.log(`OpenAPI spec saved to: ${filePath}`);
};

const execShellCommand = (cmd: string): Promise<string> =>
  new Promise((resolve, reject) => {
    exec(cmd, (error, stdout, stderr) => {
      if (error) {
        reject(error);
      }
      resolve(stdout ? stdout : stderr);
    });
  });

const runPrettierWrite = async () => {
  const output = await execShellCommand(`prettier --write ${OUTPUT_FOLDER}`);
  console.log(output);
  console.log(`All files in '${OUTPUT_FOLDER}' have been formatted using Prettier.`);
};

(async () => {
  await downloadJsonFile(OUTPUT_FOLDER);
  try {
    console.log('Formatting JSON files using Prettier.');
    await runPrettierWrite();
  } catch (error) {
    console.error(error);
    process.exit(1);
  }
})();
