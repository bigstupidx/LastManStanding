using UnityEngine;
using System.Collections;

public class BaseClass : MonoBehaviour
{

    private GameObject _gameObjectCache;

    public GameObject gameObjectCache { get { if (_gameObjectCache == null) _gameObjectCache = this.gameObject; return _gameObjectCache; } }

    private Transform _transformCache;

    public Transform transformCache { get { if (_transformCache == null) _transformCache = this.transform; return _transformCache; } }

    private SpriteRenderer _spriteRendererCache;

    public SpriteRenderer spriteRendererCache { get { if (_spriteRendererCache == null) _spriteRendererCache = this.GetComponent<SpriteRenderer>(); return _spriteRendererCache; } }

    private TrailRenderer _trailRendererCache;

    public TrailRenderer trailRendererCache { get { if (_trailRendererCache == null) _trailRendererCache = this.GetComponent<TrailRenderer>(); return _trailRendererCache; } }


    private TextMesh _textMeshCache;

    public TextMesh textMeshCache { get { if (_textMeshCache == null) _textMeshCache = this.GetComponent<TextMesh>(); return _textMeshCache; } }

    private Renderer _rendererCache;

    public Renderer rendererCache { get { if (_rendererCache == null) _rendererCache = this.renderer; return _rendererCache; } }
    
    private Camera _cameraCache;

    public Camera cameraCache { get { if (_cameraCache == null) _cameraCache = this.camera; return _cameraCache; } }
    
    private Collider2D _colliderCache2d;

    public Collider2D colliderCache2d { get { if (_colliderCache2d == null) _colliderCache2d = this.collider2D; return _colliderCache2d; } }

    private BoxCollider2D _boxColliderCache2D;

    public BoxCollider2D boxColliderCache2D { get { if (_boxColliderCache2D == null) _boxColliderCache2D = this.GetComponent<BoxCollider2D>(); return _boxColliderCache2D; } }

    private CircleCollider2D _circleColliderCache2D;

    public CircleCollider2D circleColliderCache2D { get { if (_circleColliderCache2D == null) _circleColliderCache2D = this.GetComponent<CircleCollider2D>(); return _circleColliderCache2D; } }

    private PolygonCollider2D _polygonColliderCache2D;

    public PolygonCollider2D polygonColliderCache2D { get { if (_polygonColliderCache2D == null) _polygonColliderCache2D = this.GetComponent<PolygonCollider2D>(); return _polygonColliderCache2D; } }

    private Rigidbody2D _rigidbodyCache2D;

    public Rigidbody2D rigidbodyCache2D { get { if (_rigidbodyCache2D == null) _rigidbodyCache2D = this.rigidbody2D; return _rigidbodyCache2D; } }
 
    private Animator _animatorCache;

    public Animator animatorCache { get { if (_animatorCache == null) _animatorCache = this.GetComponent<Animator>(); return _animatorCache; } }

    private ParticleSystem _particleSystemCache;

    public ParticleSystem particleSystemCache { get { if (_particleSystemCache == null) _particleSystemCache = this.GetComponent<ParticleSystem>(); return _particleSystemCache; } }

    private AudioSource _audioSourceCache;

    public AudioSource audioSourceCache { get { if (_audioSourceCache == null) _audioSourceCache = this.GetComponent<AudioSource>(); return _audioSourceCache; } }


}