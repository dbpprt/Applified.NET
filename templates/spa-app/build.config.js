exports.authorizationToken = "{A5EC5876-3C7E-4A6F-A77E-EDDB416C4437}";
//exports.authorizationToken = "{871B8E52-63FC-40CD-9392-066AD9219C03}";
exports.applicationEndpoint = "http://localhost:8080/api/deploy";
exports.outputFolder = "build";
exports.sourceFolder = "src";

exports.scriptReferences = [
    './bower_components/angular/angular.js',
    './bower_components/angular-route/angular-route.js',
    './bower_components/angular-resource/angular-resource.js',
    './bower_components/angular-animate/angular-animate.js',
    './bower_components/angular-sanitize/angular-sanitize.js',
    './bower_components/angular-loading-bar/build/loading-bar.js',
    './bower_components/jquery/dist/jquery.js',
    './bower_components/bootstrap/dist/js/bootstrap.js',
    './bower_components/angular-bootstrap/ui-bootstrap.js',
    './bower_components/angular-security/angular-spa-security.js'
];

exports.styleReferences = [
    './bower_components/angular-loading-bar/src/loading-bar.css',
    './' + this.sourceFolder + '/styles/base.less'
];

exports.assets = [
    './' + this.sourceFolder + '/assets/**/*',
    './bower_components/font-awesome/fonts/fontawesome-webfont.eot',
    './bower_components/open-sans-fontface/fonts/Light/OpenSans-Light.eot'
];

exports.templates = [
    './' + this.sourceFolder + '/views/**/*.html',
    '!./' + this.sourceFolder + '/views/index.html'
];

exports.typescriptFiles = [
    '!./bower_components/**/*',
    './' + this.sourceFolder + '/**/*.ts/',
    '!./node_modules/**/*'
];

exports.indexFile = './' + this.sourceFolder + '/views/index.html'

exports.deployRequestOptions = {
    url: this.applicationEndpoint,
    headers: {
        'AccessToken': this.authorizationToken
    }
};

