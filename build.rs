
fn main() {

    slint_build::compile("ui\\main.slint").unwrap();

    let mut res = winres::WindowsResource::new();
    res.set_icon("ui/pushgit2x32.ico");
    res.compile().unwrap();























    

}
