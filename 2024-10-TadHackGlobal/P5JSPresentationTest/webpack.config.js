const path = require('path');


module.exports = {
    entry: './src/index.ts',
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: 'bundle.js',
    },
    devtool: "source-map",
    mode: "development",
    resolve: {
        extensions: ['.ts', '.js', '.jsx', "", ".webpack.js", "web.js", "tsx"],
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/, loader: 'ts-loader',
            },
            {
                test: /\.tsx?$/, loader: 'source-map-loader',
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env'],
                    },
                },
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader',
                ]
            },
            {
                test: /\.(?:mp4|mov)$/i,
                type: 'asset/resource',
            }
        ],
    },
    devServer: {
        static: {
            directory: path.join(__dirname, 'dist'),
        },
        compress: true,
        port: 9000,
    },
    devtool: 'inline-source-map',
};