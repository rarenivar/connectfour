#lang racket

(require json)

;;; Define grid dimensions.
(define HEIGHT 6)
(define WIDTH 7)

;;; These will eventually be command line arguments.
(define exe-1 "connect-four-naive.exe")
(define exe-2 "connect-four-naive.exe")

;;; (new-grid grid move player) -> (listof (listof (integer-in 0 2)))
;;;   grid : (listof (listof (integer-in 0 2)))
;;;   move : exact-positive-integer?
;;;   player : (integer-in 1 2)
;;; Retruns a new grid with the specified move by player.
(define (new-grid grid move player)
  (for/list ((col (in-list grid))
             (i (in-naturals)))
    (cond ((= i move)
           (define n (count zero? col))
           (when (= n 0)
             (error 'new-grid
                    "player ~a move ~s is illegal" player move))
           (append (make-list (- n 1) 0) (list player) (drop col n)))
          (else
           col))))

;;; (winner? grid player) -> boolean?
;;;   grid : (listof (listof (integer-in 0 2)))
;;;   player : (integer-in 1 2)
;;; Returns true of the grid is a win for player.
(define (winner? grid player)
  (define vgrid
    (for/vector #:length WIDTH ((col (in-list grid)))
      (for/vector #:length HEIGHT ((cell (in-list col)))
        cell)))
  (let/ec exit
    (for* ((i (in-range WIDTH))
           (j (in-range HEIGHT)))
      ;; Check for four in a row diagonally up.
      (when (and (<= i (- WIDTH 4))
                 (>= j 3)
                 (for/and ((k (in-range 4)))
                   (= player (vector-ref (vector-ref vgrid (+ i k)) (- j k)))))
        (exit #t))
      ;; Check for four in a row across.
      (when (and (<= i (- WIDTH 4))
                 (for/and ((k (in-range 4)))
                   (= player (vector-ref (vector-ref vgrid (+ i k)) j))))
        (exit #t))
      ;; Check for four in a row diagonally down.
      (when (and (<= i (- WIDTH 4))
                 (<= j (- HEIGHT 4))
                 (for/and ((k (in-range 4)))
                   (= player (vector-ref (vector-ref vgrid (+ i k)) (+ j k)))))
        (exit #t))
      ;; Check for four in a row down.
      (when (and (<= j (- HEIGHT 4))
                 (for/and ((k (in-range 4)))
                   (= player (vector-ref (vector-ref vgrid i) (+ j k)))))
        (exit #t)))
    #f))

;;; (grid-print grid move) -> void?
;;;   grid : (listof (listof (integer-in 0 2)))
;;;   move : exact-positive-integer?
;;; Prints the grid.
(define (grid-print grid move)
  (printf "--- Move ~a ---~n" move)
  (for ((j (in-range HEIGHT)))
    (for ((i (in-range WIDTH)))
      (define cell (list-ref (list-ref grid i) j))
      (printf "~a " (if (= cell 0) " " cell)))
    (printf "~n")))

;;; (main) -> void?
(define (main)
  ;; Create forts for standard error (text) files.
  (define stderr-1 (open-output-file "connect-four-stderr-1.txt"
                                     #:mode 'text #:exists 'replace))
  (define stderr-2 (open-output-file "connect-four-stderr-2.txt"
                                     #:mode 'text #:exists 'replace))
  ;; Create the subprocesses.
  (define-values (process-1 process-stdout-1 process-stdin-1 process-stderr-1)
    (subprocess #f #f stderr-1 exe-1 ""))
  (sleep .1)
  (define-values (process-2 process-stdout-2 process-stdin-2 process-stderr-2)
    (subprocess #f #f stderr-2 exe-1 ""))
  ;; Create vector of standard ports.
  (define process-stdout (vector #f process-stdout-1 process-stdout-2))
  (define process-stdin (vector #f process-stdin-1 process-stdin-2))
  (define process-stderr (vector #f  stderr-1 stderr-2))
  ;; Loop alternately calling the players.
  (let/ec exit
    (let loop ((move 1)
               (player (if (<= (random) 0.5) 1 2))
               (grid (for/list ((col (in-range WIDTH)))
                       (for/list ((row (in-range HEIGHT)))
                         0))))
      ;; Construct the percept as a jsexpr.
      (define percept-jsexpr
        (hasheq 'player player
                'height HEIGHT
                'width WIDTH
                'grid grid))
      ;; Send the percept to the appropriate player.
      (displayln (jsexpr->string percept-jsexpr) (vector-ref process-stdin player))
      (flush-output (vector-ref process-stdin player))
      ;; Get the action from the appropriate player.
      (define action-json (read-line (vector-ref process-stdout player)))
      (when (eof-object? action-json)
        (exit))
      (define action-jsexpr (string->jsexpr action-json))
      ;; Process the action.
      (define next-grid (new-grid grid (hash-ref action-jsexpr 'move) player))
      (grid-print next-grid move)
      (cond ((winner? next-grid player)
             (printf "Player ~a wins.~n" player))
            ((= move (* HEIGHT WIDTH))
             (printf "Draw.~n"))
            (else
             (loop (+ move 1) (if (= player 1) 2 1) next-grid)))))
  ;; Close the ports.
  (for ((player (in-range 1 3)))
    (close-output-port (vector-ref process-stdin player))
    (close-input-port (vector-ref process-stdout player))
    (close-output-port (vector-ref process-stderr player))))

;;; Start the program.
(main)
