var gulp = require('gulp');
var gulpBowerFiles = require('gulp-bower-files');

gulp.task("bower-files", function () {
    gulpBowerFiles().pipe(gulp.dest("./TestAspNetSpa/lib"));
});

gulp.task('default', ["bower-files"], function() {
    // place code for your default task here
    
});