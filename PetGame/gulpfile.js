﻿/// <binding AfterBuild='default' Clean='clean' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var del = require('del');
var less = require('gulp-less');

var paths = {
    scripts: ['scripts/**/*.js', 'scripts/**/*.ts', 'scripts/**/*.map'],
};

gulp.task('clean', function () {
    return del(['wwwroot/scripts/**/*']);
});

gulp.task('default', function () {
    var ts = gulp.src(paths.scripts).pipe(gulp.dest('wwwroot/scripts'));
    var lesscss = gulp.src('style/main.less')
        .pipe(less())
        .pipe(gulp.dest('wwwroot/css'));
    return [ts, lesscss];
});

gulp.task('less', function () {
    return gulp.src('style/main.less')
        .pipe(less())
        .pipe(gulp.dest('wwwroot/css'));
});