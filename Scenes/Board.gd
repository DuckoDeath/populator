extends Area2D

var rand = RandomNumberGenerator.new();

enum State {STATE_EMPTY, STATE_TREE, STATE_HOUSE, STATE_WINDMILL, STATE_FACTORY, STATE_APARTMENT}

var state = State.STATE_EMPTY; # default state

func _ready():
	rand.randomize();
	var color_choice = rand.randi_range(0, 1)

	if color_choice == 0:
		$ColorRect.color = Color8(51, 152, 75)
	else:
		$ColorRect.color = Color8(30, 111, 80)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

