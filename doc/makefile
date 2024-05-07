all : ascii-art-for-reds.pdf ascii-art-for-blues.pdf caesar-for-reds.pdf caesar-for-blues.pdf \
      huffman-for-reds.pdf huffman-for-blues.pdf number-system-for-reds.pdf number-system-for-blues.pdf

ascii-art-for-reds.pdf : ascii-art-for-reds.tex  
	xelatex ascii-art-for-reds.tex

ascii-art-for-blues.pdf : ascii-art-for-blues.tex
	xelatex ascii-art-for-blues.tex

caesar-for-reds.pdf : caesar-for-reds.tex
	xelatex caesar-for-reds.tex

caesar-for-blues.pdf : caesar-for-blues.tex
	xelatex caesar-for-blues.tex

huffman-for-reds.pdf : huffman-for-reds.tex huffman_abc.svg
	xelatex --shell-escape huffman-for-reds.tex

huffman-for-blues.pdf : huffman-for-blues.tex huffman_abc.svg
	xelatex --shell-escape huffman-for-blues.tex

number-system-for-reds.pdf : number-system-for-reds.tex
	xelatex number-system-for-reds.tex

number-system-for-blues.pdf : number-system-for-blues.tex
	xelatex number-system-for-blues.tex

clean:
	rm *.aux *.log *.pdf
	rm -rf svg-inkspace

cleantmp:
	rm *.aux *.log
