extends Node

var game = []

# Called when the node enters the scene tree for the first time.
func _ready():
	# generate a game array
	for i in 4:
		var arr = []
		for j in 4:
			var board = load("res://Scenes/Board.tscn")
			var board_instance = board.instance()
			add_child(board_instance)

			# move into position
			var board_pos = Vector2(j*16, i*16)
			board_instance.position = board_pos

			arr.push_back(board_instance)
		game.push_back(arr)


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
