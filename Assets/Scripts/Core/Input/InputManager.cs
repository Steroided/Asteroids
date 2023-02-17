using Asteroids.Core;
using Asteroids.Objects;

namespace Asteroids.Core
{
    public class InputManager
    {
        private PlayerInput _input { get; }
        private Player _player { get; }
        public InputManager(Player player)
        {
            _input = new PlayerInput();

            _player = player;
            _input.Player.ShootBullet.performed += context => _player.Shoot(0);
            _input.Player.ShootLazer.performed += context => _player.Shoot(1);
            _input.Enable();
        }
        public void Update()
        {
            _player._moveInput = _input.Player.Move.ReadValue<float>();
            _player._turnInput = _input.Player.Rotate.ReadValue<float>();
            GamePlayManager._newGameKey = _input.Player.NewGame.IsPressed();
        }
    }
}  