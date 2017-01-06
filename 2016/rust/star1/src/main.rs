use std::fs::File;
use std::io::Read;

enum Direction {
    North,
    South,
    East,
    West
}

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

    println!("Final distance moved: {}", data.get_taxi_coordinate_length());
}

struct Data {
    current_direction : Direction,
    horizontal_movement : i32,
    vertical_movement : i32
}

impl Data {
    fn new() -> Data {
        Data { current_direction : Direction::North, horizontal_movement: 0, vertical_movement: 0 }
    }
    fn update_current_direction( &mut self, new_direction : char ) {

        if new_direction == 'R' {
            self.current_direction = match self.current_direction {
                Direction::North => Direction::East,
                Direction::East => Direction::South,
                Direction::South => Direction::West,
                Direction::West => Direction::North
            }
        } else if new_direction == 'L' {
            self.current_direction = match self.current_direction {
                Direction::North => Direction::West,
                Direction::East => Direction::North,
                Direction::South => Direction::East,
                Direction::West => Direction::South
            }
        } else {
            panic!("Unknown direction!");
        }
    }

    fn walk( &mut self, distance : u32 ) {
        match self.current_direction {
            Direction::North => self.vertical_movement += distance as i32,
            Direction::South => self.vertical_movement -= distance as i32,
            Direction::East => self.horizontal_movement += distance as i32,
            Direction::West => self.horizontal_movement -= distance as i32,
        }
    }

    fn get_taxi_coordinate_length(&self) -> i32 {
        let length = self.horizontal_movement.abs() + self.vertical_movement.abs();
        length
    }
}
