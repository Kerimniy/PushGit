#![windows_subsystem = "windows"]

use slint::SharedString;
use std::process::Command;
use slint::{ CloseRequestResponse, invoke_from_event_loop, ComponentHandle};
use std::{fs};
use serde_json;
use std::thread;
use std::io::Write;
#[cfg(target_os = "windows")]
use std::os::windows::process::CommandExt;

slint::include_modules!();
// Base template

/*

config structure:
	1 git commands, \n - new line
	2 using force mode to push
	3 using custom config. if true (1) using raw first row
	4-10  values for format (n [] will be replaced by n var)

*/

const  BASE_TEMPLATE: &str = r##"
    [

		"cd [] \ngit init\ngit add . \ngit commit -m '[]' \ngit config --global --unset url.gitgithub.com:.insteadOf \ngit remote rm origin \ngit remote add origin []  \ngit branch -M []  \ngit fetch origin []  \ngit pull --rebase origin []  \ngit push origin [] [] ",
		"0",
		"0",
		"",
		"commit0",
		"",
		"main",
		"main",
		"main",
		"main",
		""

]
    "##;



fn read_file_content_as_string(path: &str) -> String {

	
    let str_content =match fs::read_to_string(path){
        Ok(string_content) => string_content,
        Err(_) => BASE_TEMPLATE.to_string()
    };

	
    str_content
}



fn execute_command(s: &String) -> String{
	
	//Windows shell
    #[cfg(target_os = "windows")]
    {
        let output = Command::new("powershell")
            .args(&["/C", s])
            .creation_flags(0x08000000)
            .output()
            .expect("failed to execute process");

        let exec_result: String =match String::from_utf8(  output.stdout) {
            Ok(res) => res,
            Err(err) => String::from(err.to_string())
        };
        return exec_result;
    }

    #[cfg(not(target_os = "windows"))]
    {
		//Unix-like os shell
        let output = Command::new("sh")
            .arg("-c")
            .arg(s)
            .output()
            .expect("failed to execute process");

        let exec_result: String =match String::from_utf8(  output.stdout) {
            Ok(res) => res,
            Err(err) => String::from(err.to_string())
        };
        return exec_result;
    }



}




fn main() -> Result<(), Box<dyn std::error::Error>> {
//create window
    let app = App::new().unwrap();
    let about_window = AboutWindow::new()?;
    about_window.hide();
    let weak_about = about_window.as_weak();

	
    let read = read_file_content_as_string(&"config.json");

	
    let json_data: Result<Vec<String>, serde_json::Error> = serde_json::from_str(&read);

	
    let mut json_data = match json_data {
        Ok(v) => v,
        Err(_) =>serde_json::from_str::<Vec<String>>(BASE_TEMPLATE).unwrap()
    };


    app.on_show_about(move || {
        if let Some(w) = weak_about.upgrade() {
            if let Err(e) = w.show() {
            }
        }
    });
    let weak_about = about_window.as_weak();
    about_window.on_hide_about(move || {
        if let Some(w) = weak_about.upgrade()
        {
            if let Err(e) = w.hide() {
            }
        }
    });
//load data

	
    app.set_is_forcemode(json_data[1].clone().parse::<usize>().unwrap() !=0);
    app.set_is_custom_config(json_data[2].clone().parse::<usize>().unwrap() !=0);
    if !app.get_is_custom_config() {
        app.set_url(json_data[5].clone().into());
        app.set_path(json_data[3].clone().into());
        app.set_commit(json_data[4].clone().into());
        app.set_branch(json_data[6].clone().into());
    }

	
//slint callbacks
    let weak_app = app.as_weak().unwrap();
    app.on_select_folder(move || {let _path: SharedString = pick_folder(&weak_app.get_path());  weak_app.set_path(_path.clone());});


	
    let weak_app = app.as_weak().unwrap();
    app.on_forcemode_changed(move |val| {weak_app.set_is_forcemode(val)});


	
    let weak_app = app.as_weak().unwrap();
    app.on_use_custom_changed(move |val| {weak_app.set_is_custom_config(val)});


	
    let weak_app = app.as_weak().unwrap();
    app.on_change_branch(move |string| { weak_app.set_branch(string);});


	
    let weak_app = app.as_weak().unwrap();
    app.on_change_commit(move |string| { weak_app.set_commit(string);});


	
    let weak_app = app.as_weak().unwrap();
    app.on_change_url(move |string| { weak_app.set_url(string);});


	
    let weak_app = app.as_weak().unwrap();
    app.on_change_path(move |string| { weak_app.set_path(string);});



	
    let weak_app = app.as_weak().unwrap();


	
    app.on_push(move || {

        let weak_app1 = weak_app.as_weak();

        let read = read_file_content_as_string(&"config.json");

        let json_data: Result<Vec<String>, serde_json::Error> = serde_json::from_str(&read);

        let mut json_data = match json_data {
            Ok(v) => v,
            Err(_) => serde_json::from_str::<Vec<String>>(BASE_TEMPLATE).unwrap()
        };
        weak_app.set_logs(format!("{} \n Attempting to push... \n",weak_app.get_logs().to_string()).into());
// format template
        json_data[1] = (weak_app.get_is_forcemode() as usize).to_string();
        json_data[2] = (weak_app.get_is_custom_config() as usize).to_string();
        if !weak_app.get_is_custom_config() {
            json_data[3] = weak_app.get_path().to_string();
            json_data[4] = weak_app.get_commit().to_string();
            json_data[5] = weak_app.get_url().to_string();
            json_data[6] = weak_app.get_branch().to_string();
            json_data[7] = weak_app.get_branch().to_string();
            json_data[8] = weak_app.get_branch().to_string();
            json_data[9] = weak_app.get_branch().to_string();
            if json_data[1] == "0" {
                json_data[10] = String::new();
            } else {
                json_data[10]="-f".to_string();
            }
        }

		
        let  base_command = json_data[0].clone();
        let format_count: usize = json_data[0].matches("[]").count();
        for i in 3..=format_count+2{
            if i < json_data.len(){
                json_data[0] = json_data[0].replacen("[]", &json_data[i],1);

            }
            else {
                json_data[0] =json_data[0].replacen("[]", "",1);
            }

        }
        let command = json_data[0].clone();
        let logsimm = weak_app.get_logs();
		//executing commands in other process
        thread::spawn(move || {
            let mut logs = logsimm.clone();
            let result = execute_command(&command);
            logs.push_str(&result);
            if let Err(e) = invoke_from_event_loop(move || {
                if let Some(app) = weak_app1.upgrade() {
                    app.set_logs(logs.into());
                } else {
                }
            }) {
            }
        });

		

        json_data[0] = base_command;
        let json_string = serde_json::to_string_pretty(&json_data).unwrap();
		saving cconfig
        let mut file = std::fs::File::create("config.json").unwrap();

        file.write(json_string.as_bytes());


    });



//saving config on exet
    let weak_app = app.as_weak().unwrap();
    app.window().on_close_requested(move || {
        let read = read_file_content_as_string(&"config.json");

        let json_data: Result<Vec<String>, serde_json::Error> = serde_json::from_str(&read);

        let mut json_data = match json_data {
            Ok(v) => v,
            Err(_) => Vec::new()
        };



        json_data[1] = (weak_app.get_is_forcemode() as usize).to_string();
        json_data[2] = (weak_app.get_is_custom_config() as usize).to_string();
        if !weak_app.get_is_custom_config() {
            json_data[3] = weak_app.get_path().to_string();
            json_data[4] = weak_app.get_commit().to_string();
            json_data[5] = weak_app.get_url().to_string();
            json_data[6] = weak_app.get_branch().to_string();
            json_data[7] = weak_app.get_branch().to_string();
            json_data[8] = weak_app.get_branch().to_string();
            json_data[9] = weak_app.get_branch().to_string();
            if json_data[1] == "0" {
                json_data[10] = String::new();
            } else {
                json_data[10] = "-f".to_string();
            }


			
            let json_string = serde_json::to_string_pretty(&json_data).unwrap();
            let mut file = std::fs::File::create("config.json").unwrap();

            file.write(json_string.as_bytes());
        }
		
        CloseRequestResponse::HideWindow

		
    });



//callback reset template rewrite your config.json to defoult
    app.on_reset_template(move || {

        let mut file = std::fs::File::create("config.json").unwrap();

        file.write(BASE_TEMPLATE.to_string().as_bytes());

    });




    app.run()?;
    Ok(())



		
}

		
		//Open File dialog

fn pick_folder(s : &SharedString) -> SharedString{
    let fd = rfd::FileDialog::new().set_title("Select Directory").pick_folder();

    if fd.is_some() {
        return  fd.unwrap().display().to_string().into();
    }
    else {
        return s.clone();
    }


		

}
