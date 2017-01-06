use coordinates::Coordinates;

enum Direction {
    North,
    South,
    East,
    West
}

pub struct Data {
    current_direction : Direction,
    total_coordinates : Coordinates,
    visited_locations : Vec<Coordinates>
}

impl Data {
    pub fn new() -> Data {
        Data { current_direction : Direction::North, total_coordinates : Coordinates::new(), visited_locations : Vec::new() }
    }
    pub fn update_current_direction( &mut self, new_direction : char ) {

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

    pub fn walk( &mut self, distance : u32 ) {
        match self.current_direction {
            Direction::North => self.total_coordinates.vertical_movement += distance as i32,
            Direction::South => self.total_coordinates.vertical_movement -= distance as i32,
            Direction::East => self.total_coordinates.horizontal_movement += distance as i32,
            Direction::West => self.total_coordinates.horizontal_movement -= distance as i32,
        }
    }

    pub fn get_taxicab_coord_destination(&self) -> i32 {
        let length = self.total_coordinates.horizontal_movement.abs() + self.total_coordinates.vertical_movement.abs();
        length
    }
}
