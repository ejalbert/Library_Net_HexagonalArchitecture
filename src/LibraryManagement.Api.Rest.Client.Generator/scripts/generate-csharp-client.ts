import * as fs from 'fs';
import * as path from 'path';
import { execSync } from 'child_process';
import { fileURLToPath } from 'url';

// Get __dirname equivalent for ESM
const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// Path to Directory.Build.props relative to this script's location
const DIRECTORY_BUILD_PROPS_PATH = path.resolve(__dirname, '../../../Directory.Build.props');
const BASE_CONFIG_PATH = path.resolve(__dirname, '../csharp-generator-config.json');
const TEMP_CONFIG_PATH = path.resolve(__dirname, '../csharp-generator-config.temp.json');

/**
 * Extracts the LibraryManagementVersion from Directory.Build.props
 */
function getVersionFromDirectoryBuildProps(): string {
  console.log(`Reading version from: ${DIRECTORY_BUILD_PROPS_PATH}`);

  if (!fs.existsSync(DIRECTORY_BUILD_PROPS_PATH)) {
    throw new Error(`Directory.Build.props not found at: ${DIRECTORY_BUILD_PROPS_PATH}`);
  }

  const content = fs.readFileSync(DIRECTORY_BUILD_PROPS_PATH, 'utf-8');

  // Extract LibraryManagementVersion using regex
  const versionMatch = content.match(/<LibraryManagementVersion>([^<]+)<\/LibraryManagementVersion>/);

  if (!versionMatch || !versionMatch[1]) {
    throw new Error('LibraryManagementVersion not found in Directory.Build.props');
  }

  const version = versionMatch[1].trim();
  console.log(`Found version: ${version}`);
  return version;
}

/**
 * Creates a temporary config file with the version injected
 */
function createTempConfigWithVersion(version: string): void {
  const baseConfig = JSON.parse(fs.readFileSync(BASE_CONFIG_PATH, 'utf-8'));
  baseConfig.packageVersion = version;
  fs.writeFileSync(TEMP_CONFIG_PATH, JSON.stringify(baseConfig, null, 2));
  console.log(`Created temp config with version ${version}`);
}

/**
 * Cleans up the temporary config file
 */
function cleanupTempConfig(): void {
  if (fs.existsSync(TEMP_CONFIG_PATH)) {
    fs.unlinkSync(TEMP_CONFIG_PATH);
    console.log('Cleaned up temp config');
  }
}

/**
 * Runs the OpenAPI Generator CLI for C# with the extracted version
 */
function generateCSharpClient(version: string): void {
  console.log(`Generating C# client with version: ${version}`);

  // Create temp config with version
  createTempConfigWithVersion(version);

  // Build the full command - not using --generator-key to allow config override
  const command = [
    'npx openapi-generator-cli generate',
    '-g csharp',
    '-i openapi-specs/openapi.json',
    '-o csharp-client',
    '-c csharp-generator-config.temp.json'
  ].join(' ');

  console.log(`Executing: ${command}`);

  try {
    execSync(command, {
      stdio: 'inherit',
      cwd: path.resolve(__dirname, '..')
    });
    console.log('C# client generation completed successfully');
  } catch (error) {
    console.error('Failed to generate C# client:', error);
    throw error;
  } finally {
    cleanupTempConfig();
  }
}

// Main execution
(async () => {
  try {
    const version = getVersionFromDirectoryBuildProps();
    generateCSharpClient(version);
  } catch (error) {
    console.error('Error:', error);
    process.exit(1);
  }
})();





