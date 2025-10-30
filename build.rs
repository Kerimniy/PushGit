
fn main() {

    //compile .slint file
    slint_build::compile("ui\\main.slint").unwrap();

    // install icon (Windows)
    let mut res = winres::WindowsResource::new();
    res.set_icon("ui/pushgit2x32.ico");
    res.compile().unwrap();





























































    

}
