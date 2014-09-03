var gulp = require('gulp');
var mainBowerFiles = require('main-bower-files');

gulp.task("bower-files", function () {
    return gulp.src(mainBowerFiles())
        .pipe(gulp.dest("./TestAspNetSpa/lib"));
});

gulp.task('default', ["bower-files"], function() {
    // place code for your default task here
    
});