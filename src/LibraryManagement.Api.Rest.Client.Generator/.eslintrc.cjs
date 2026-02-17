module.exports = {
  root: true,
  env: { es2020: true },
  extends: ['eslint:recommended', 'plugin:@typescript-eslint/recommended'],
  ignorePatterns: ['.eslintrc.cjs', 'src/**'],
  parser: '@typescript-eslint/parser',
  plugins: ['jsonc'],
  rules: {
    '@typescript-eslint/no-explicit-any': 'off',
    '@typescript-eslint/no-var-requires': 'off',
    '@typescript-eslint/no-unsafe-declaration-merging': 'off',
  },
  overrides: [
    {
      files: ['openapi-specs/*.json'],
      parser: 'jsonc-eslint-parser',
      rules: {
        'jsonc/sort-keys': [
          'warn',
          {
            pathPattern: '.*',
            order: { type: 'asc' },
          },
        ],
      },
    },
  ],
};
