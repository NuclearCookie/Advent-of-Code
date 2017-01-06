use coordinate::Coordinate;

enum Direction {
    North,
    South,
    East,
    West
}

pub struct Data {
    current_direction : Direction,
    total_coordinates : Coordinate,
    current_coordinate : Coordinate,
    visited_locations : Vec<Coordinate>
}

impl Data {
    pub fn new() -> Data {
        Data {
            current_direction : Direction::North,
            total_coordinates : Coordinate::new(),
            current_coordinate : Coordinate::new(),
            visited_locations : Vec::new()
        }
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
        let distance = distance as i32;
        let previous_coords = self.current_coordinate.clone();
        for _ in 0..distance {
            match self.current_direction {
                Direction::North => {
                        self.current_coordinate.vertical_movement += 1;
                        self.visited_locations.push(self.current_coordinate.clone());
                },
                Direction::South => {
                        self.current_coordinate.vertical_movement -= 1;
                        self.visited_locations.push(self.current_coordinate.clone());
                },
                Direction::East => {
                        self.current_coordinate.horizontal_movement += 1;
                        self.visited_locations.push(self.current_coordinate.clone());
                },
                Direction::West => {
                        self.current_coordinate.horizontal_movement -= 1;
                        self.visited_locations.push(self.current_coordinate.clone());
                },
            }
        }
        // :NOTE: Why do I have to do this?
        let current_coordinate_copy = self.current_coordinate.clone();
        // :NOTE: --end
        self.total_coordinates += current_coordinate_copy - previous_coords;
    }

    pub fn get_taxicab_coord_destination(&self) -> i32 {
        let length = self.total_coordinates.horizontal_movement.abs() + self.total_coordinates.vertical_movement.abs();
        length
    }
}
