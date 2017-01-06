use std::fs::File;
use std::io::Read;

mod data;
mod coordinate;

use data::Data;

fn main() {
    let mut file = File::open("input/input.txt")
        .expect("Failed to open file");

    let mut content = String::new();

    file.read_to_string(&mut content)
        .expect("Failed to read content of file");

    let list_of_commands: Vec<&str> = content.trim().split(", ").collect();

    let mut data = Data::new();

    for command in list_of_commands {
        let ( direction, length ) = command.split_at(1);
        data.update_current_direction( match direction.chars().next() {
            Some(x) => x,
            None => panic!("Invalid input!")
        });
        data.walk( match length.parse() {
            Ok(x) => x,
            Err(_) => panic!("Invalid input!")
        } );
    }

    println!("Final distance moved: {}", Data::get_taxicab_coord_destination(&data.total_coordinates));
}
