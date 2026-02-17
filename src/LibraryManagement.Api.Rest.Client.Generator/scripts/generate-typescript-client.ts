import * as fs from 'fs';
import * as path from 'path';
import { execSync } from 'child_process';
import { fileURLToPath } from 'url';

// Get __dirname equivalent for ESM
const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// Paths
const DIRECTORY_BUILD_PROPS_PATH = path.resolve(__dirname, '../../../Directory.Build.props');
const PACKAGE_JSON_PATH = path.resolve(__dirname, '../package.json');

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
 * Updates the version in package.json
 */
function updatePackageJsonVersion(version: string): void {
  console.log(`Updating package.json version to: ${version}`);

  const packageJson = JSON.parse(fs.readFileSync(PACKAGE_JSON_PATH, 'utf-8'));
  packageJson.version = version;

  fs.writeFileSync(PACKAGE_JSON_PATH, JSON.stringify(packageJson, null, 2) + '\n');
  console.log('package.json version updated successfully');
}

/**
 * Runs the OpenAPI Generator CLI for TypeScript
 */
function generateTypeScriptClient(): void {
  console.log('Generating TypeScript client...');

  const command = 'npx openapi-generator-cli generate --generator-key typescript';

  console.log(`Executing: ${command}`);

  try {
    execSync(command, {
      stdio: 'inherit',
      cwd: path.resolve(__dirname, '..')
    });
    console.log('TypeScript client generation completed successfully');
  } catch (error) {
    console.error('Failed to generate TypeScript client:', error);
    throw error;
  }
}

// Main execution
(async () => {
  try {
    const version = getVersionFromDirectoryBuildProps();
    updatePackageJsonVersion(version);
    generateTypeScriptClient();
  } catch (error) {
    console.error('Error:', error);
    process.exit(1);
  }
})();

