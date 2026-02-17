import resolve from '@rollup/plugin-node-resolve';
import commonjs from '@rollup/plugin-commonjs';
import typescript from 'rollup-plugin-typescript2';
import nodePolyfills from 'rollup-plugin-node-polyfills';
import terser from '@rollup/plugin-terser';
import json from '@rollup/plugin-json';
import packageJson from './package.json' assert { type: 'json' };

export default {
  input: 'src/index.ts',
  output: [
    {
      name: packageJson.name,
      file: `dist/index.mjs`,
      sourcemap: true,
      exports: 'named',
      format: 'esm',
      banner: `'use client';`,
    },
    {
      name: packageJson.name,
      file: `dist/index.cjs`,
      sourcemap: true,
      exports: 'named',
      format: 'cjs',
      banner: `'use client';`,
    },
    {
      name: packageJson.name,
      file: `dist/index.umd.js`,
      sourcemap: true,
      exports: 'named',
      format: 'umd',
      banner: `'use client';`,
    },
  ],
  plugins: [
    resolve({
      browser: true, // Instructs the plugin to use browser-friendly versions
      preferBuiltins: false,
    }), // so Rollup can find `node_modules`
    typescript({
      useTsconfigDeclarationDir: true,
    }), // so Rollup can compile TypeScript
    commonjs(), // so Rollup can convert `node_modules` to an ES module
    terser(), // Minify the bundle
    json(),
    nodePolyfills(),
  ],
};
